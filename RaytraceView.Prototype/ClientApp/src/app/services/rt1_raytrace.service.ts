import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Rt1Controls } from '../rt1_in_one_weekend/rt1_controls/rt1_controls.component';
import { RayTraceView } from '../shared/interfaces/IRayTraceView';
import { Rt1InOneWeekend, Rt1InOneWeekendImageData } from './model/rt1_models';
import { ShaderProgram } from "../shared/webgl/ShaderProgram";
import { VertexArrayObject } from "../shared/webgl/VertexArrayObject";
import { Texture } from "../shared/webgl/Texture";
import { PixelData } from '../shared/model/type_models';

@Injectable()
export class Rt1Service {
  public controls: Rt1Controls = null;
  public view: RayTraceView = null;
  private raytraceimage: HTMLImageElement;
  private gl: any;
  private progDraw: ShaderProgram;
  private bufQuad: VertexArrayObject;
  private texture: Texture;
  private running = false;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { }

  public startRayTrace(): void {
    if (!this.view)
      return;
    this.running = false;
    this.initRayTrace((raytrace: Rt1InOneWeekend) => {
      this.initView(raytrace.imagePng);
    });
  }

  public stopRayTrace(): void {
    this.running = false;
  }

  public initRayTrace(setRayTrace: (raytrace: Rt1InOneWeekend) => void): void {
    this.http.get<Rt1InOneWeekend>(this.baseUrl + 'rt1inoneweekend').subscribe(result => {
      setRayTrace(result);
    }, error => console.error(error));
  }

  public initView(image: string): void {
    this.running = false;
    const raytraceImageName = "data:image/png;base64," + image;
    this.raytraceimage = new Image();
    this.raytraceimage.onload = (): void => { this.createTexture(this.raytraceimage); }
    this.raytraceimage.src = raytraceImageName;
  }

  private createTexture(raytraceimage: any): void {
    this.texture = new Texture(this.gl, raytraceimage);
    this.running = true;
    requestAnimationFrame((deltaMS: number) => { this.updateData(deltaMS); });
  }

  private updateData(deltaMS: number): void {
    if (!this.running || !this.view)
      return;

    this.http.get<Rt1InOneWeekendImageData>(this.baseUrl + 'rt1inoneweekendimagedata').subscribe(result => {
      const pixelData = result.pixelData;
      if (pixelData) {
        this.texture.bind(0);
        for (let i = 0; i < pixelData.length; ++i) {
          const data: PixelData = pixelData[i];
          this.gl.texSubImage2D(this.gl.TEXTURE_2D, 0, data.x, data.y, 1, 1, this.gl.RGBA, this.gl.UNSIGNED_BYTE,
            new Uint8Array([data.r, data.g, data.b, 255]));
        }
        this.view.setProgress(result.progress);
      }
    }, error => console.error(error));

    this.refreshCanvas();

    setTimeout(() => {
      requestAnimationFrame((deltaMS: number) => { this.updateData(deltaMS); });
    }, 200);
  }

  private refreshCanvas(): void {
    requestAnimationFrame((deltaMS: number) => { this.drawCanvas(deltaMS); });
  }

  public initCanvas(): void {
    if (!this.view)
      return;
    const canvas = this.view.getCanvas();
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
    const inPos = this.progDraw.attrI("inPos");
    const inUV = this.progDraw.attrI("inUV");
    
    this.bufQuad = new VertexArrayObject(this.gl,
      [{ data: [-1, -1, 0, 1, -1, 0, 1, 1, 0, -1, 1, 0], attrSize: 3, attrLoc: inPos },
        { data: [0, 0, 1, 0, 1, 1, 0, 1], attrSize: 2, attrLoc: inUV }],
      [0, 1, 2, 0, 2, 3]);
  }

  private drawCanvas(deltaMS: number): void {
    if (!this.view || !this.progDraw || !this.bufQuad)
      return;

    const canvas = this.view.getCanvas();
    this.gl.viewport(0, 0, canvas.width, canvas.height);
    this.gl.enable(this.gl.DEPTH_TEST);
    this.gl.clearColor(1.0, 0.0, 0.0, 1.0);
    this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

    // bind the texture
    if (this.texture !== undefined)
      this.texture.bind(0);

    // set up draw shader
    this.progDraw.use();
    this.progDraw.setI1("u_texture", 0);

    // draw scene
    this.bufQuad.draw();
  }
}
