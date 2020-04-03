import { Component, Inject, OnInit } from '@angular/core';
import { Input, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ShaderProgram } from "../../shared/webgl/ShaderProgram";
import { VertexArrayObject } from "../../shared/webgl/VertexArrayObject";
import { Texture } from "../../shared/webgl/Texture";
import { Rt1InOneWeekend, Rt1InOneWeekendImageData } from '../model/rt1_models'

@Component({
  selector: 'app-rt1_in_one_weekend-rt1_view',
  templateUrl: './rt1_view.component.html',
})
export class Rt1View implements OnInit {
  @Input() raytrace: Rt1InOneWeekend;
  @ViewChild('ray_trace_canvas', { static: true }) raytracecanvas: ElementRef<HTMLCanvasElement>;
  private raytraceimage: any;
  private canvasSize: number[];
  private gl: any;
  private progDraw: any;
  private bufQuad: any;
  private texture: any;
  private progressText: string;
  private running: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    const raytraceImageName = "data:image/png;base64," + this.raytrace.imagePng;
    this.raytraceimage = new Image();
    this.raytraceimage.onload = (): void => { this.createTexture(this.raytraceimage); }
    this.raytraceimage.src = raytraceImageName;
  }

  ngOnDestroy(): void {
    this.running = false;
  }

  ngAfterViewInit(): void {
    this.initCanvas();
    window.onresize = () => { this.resize(); }
  }

  resize(): void {
    const canvas = this.raytracecanvas.nativeElement;
    this.canvasSize = [canvas.width, canvas.height];
  }

  refreshCanvas(): void {
    requestAnimationFrame((deltaMS: number) => { this.drawCanvas(deltaMS); });
  }

  createTexture(raytraceimage: any): void {
    this.texture = new Texture(this.gl, raytraceimage);
    this.running = true;
    requestAnimationFrame((deltaMS: number) => { this.updateData(deltaMS); });
  }

  updateData(deltaMS: number): void {
    if (!this.running)
      return;
    this.http.get<Rt1InOneWeekendImageData>(this.baseUrl + 'rt1inoneweekendimagedata').subscribe(result => {
      const pixelData = result.pixelData;
      if (pixelData) {
        this.texture.bind(0);
        for (let i = 0; i < pixelData.length; ++i) {
          const data = pixelData[i];
          this.gl.texSubImage2D(this.gl.TEXTURE_2D, 0, data.x, data.y, 1, 1, this.gl.RGBA, this.gl.UNSIGNED_BYTE,
            new Uint8Array([data.r, data.g, data.b, 255]));
        }
        this.progressText = (Math.round(result.progress * 100000) / 1000).toString() + "%";
      }
    }, error => console.error(error));
    this.refreshCanvas();

    setTimeout(() => {
      requestAnimationFrame((deltaMS: number) => { this.updateData(deltaMS); });
    }, 200);
  }

  initCanvas(): void {
    const canvas = this.raytracecanvas.nativeElement;
    this.canvasSize = [canvas.width, canvas.height];
    this.gl = canvas.getContext("webgl2");

    const vertexShader: any = `
    precision highp float;

    attribute vec3 inPos;
    attribute vec2 inUV;

    varying vec2 vertUV;

    void main()
    {
      vertUV = inUV;
      gl_Position = vec4(inPos, 1.0);
    }`

    const fragmentShader: any = `
    precision mediump float;

    varying vec2 vertUV;
    uniform sampler2D u_texture;

    void main()
    {
        vec4 texColor = texture2D(u_texture, vertUV.st);
        gl_FragColor  = vec4(texColor.rgb, 1.0);
    }`

    this.progDraw = new ShaderProgram(this.gl,
      [{ source: vertexShader, stage: this.gl.VERTEX_SHADER },
      { source: fragmentShader, stage: this.gl.FRAGMENT_SHADER }
      ]);
    if (!this.progDraw.progObj)
      return null;
    this.progDraw.inPos = this.progDraw.attrI("inPos");
    this.progDraw.inUV = this.progDraw.attrI("inUV");
    this.progDraw.invalid = true;

    this.bufQuad = new VertexArrayObject(this.gl,
      [{ data: [-1, -1, 0, 1, -1, 0, 1, 1, 0, -1, 1, 0], attrSize: 3, attrLoc: this.progDraw.inPos },
      { data: [0, 0, 1, 0, 1, 1, 0, 1], attrSize: 2, attrLoc: this.progDraw.inUV }],
      [0, 1, 2, 0, 2, 3]);
  }

  drawCanvas(deltaMS: number): void {
    this.gl.viewport(0, 0, this.canvasSize[0], this.canvasSize[1]);
    this.gl.enable(this.gl.DEPTH_TEST);
    this.gl.clearColor(1.0, 0.0, 0.0, 1.0);
    this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

    // bind the texture
    if (this.texture !== undefined)
      this.texture.bind(0);

    // set up draw shader
    if (this.progDraw.invalid) {
      this.progDraw.invalid = false;
      this.progDraw.use();
      this.progDraw.setI1("u_texture", 0);
    }

    // draw scene
    this.bufQuad.draw();
  }
}

