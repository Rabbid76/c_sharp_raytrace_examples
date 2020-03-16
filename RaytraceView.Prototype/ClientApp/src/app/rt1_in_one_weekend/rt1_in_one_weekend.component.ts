import { Component, Inject, ElementRef, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ShaderProgram, ShaderSpec } from "./ShaderProgram";
import { VertexBuffer, VertexAttribute } from "./VertexBuffer";
import { Texture } from "./Texture";

@Component({
  selector: 'app-rt1_in_one_weekend',
  templateUrl: './rt1_in_one_weekend.component.html',
})
export class Rt1InOneWeekendComponent {
  //@ViewChild('raytracecanvas', { static: false }) raytracecanvas: ElementRef;
  //@ViewChild('raytraceimage', { static: false }) raytraceimage: ElementRef;
  raytracecanvas: any; 
  raytraceImageName: string;
  raytrace: Rt1InOneWeekend;
  canvasSize: number[];
  gl: any;
  progDraw: any;
  bufQuad: any;
  texture: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    http.get<Rt1InOneWeekend>(baseUrl + 'rt1inoneweekend').subscribe(result => {
      this.raytrace = result;
      this.raytraceImageName = "data:image/png;base64," + this.raytrace.imagePng;
      let raytraceimage: any = document.getElementById("raytraceimage");
      raytraceimage.src = this.raytraceImageName;
    }, error => console.error(error));
  }

  ngAfterViewInit(): void {
    this.initCanvas();
    var self = this;
    window.onresize = function () { self.resize(); }
    this.refreshCanvas();
  }

  resize(): void {
    this.canvasSize = [this.raytracecanvas.width, this.raytracecanvas.height];
  }

  refreshCanvas(): void {
    var self = this;
    requestAnimationFrame((deltaMS: number) => { self.drawCanvas(deltaMS); });
  }

  initCanvas(): void {
    this.raytracecanvas = document.getElementById("raytracecanvas");
    this.canvasSize = [this.raytracecanvas.width, this.raytracecanvas.height];
    this.gl = this.raytracecanvas.getContext("experimental-webgl");

    let vertexShader: any = `
    precision highp float;

    attribute vec3 inPos;
    attribute vec2 inUV;

    varying vec2 vertUV;

    void main()
    {
      vertUV = inUV;
      gl_Position = vec4(inPos, 1.0);
    }`

    let fragmentShader: any = `
    precision mediump float;

    varying vec2 vertUV;
    uniform sampler2D u_texture;

    void main()
    {
        vec4 texColor = texture2D(u_texture, vertUV.st);
        gl_FragColor  = vec4(texColor.rgb, 1.0);
    }`

    var self = this;
    let refresh = () => { self.refreshCanvas(); };
    this.texture = new Texture(this.gl, "https://raw.githubusercontent.com/Rabbid76/graphics-snippets/master/resource/texture/supermario.jpg", refresh);
    this.texture.bound = false;

    this.progDraw = new ShaderProgram(this.gl,
      [{ source: vertexShader, stage: this.gl.VERTEX_SHADER },
        { source: fragmentShader, stage: this.gl.FRAGMENT_SHADER }
      ]);
    if (!this.progDraw.progObj)
      return null;
    this.progDraw.inPos = this.progDraw.attrI("inPos");
    this.progDraw.inUV = this.progDraw.attrI("inUV");
    this.progDraw.invalid = true;

    this.bufQuad = new VertexBuffer(this.gl,
      [{ data: [-1, -1, 0, 1, -1, 0, 1, 1, 0, -1, 1, 0], attrSize: 3, attrLoc: this.progDraw.inPos },
      { data: [0, 0, 1, 0, 1, 1, 0, 1], attrSize: 2, attrLoc: this.progDraw.inUV }],
      [0, 1, 2, 0, 2, 3]);
  }

  drawCanvas(deltaMS: number): void {
    this.gl.viewport(0, 0, this.canvasSize[0], this.canvasSize[1]);
    this.gl.enable(this.gl.DEPTH_TEST);
    this.gl.clearColor(1.0, 0.0, 0.0, 1.0);
    this.gl.clear(this.gl.COLOR_BUFFER_BIT | this.gl.DEPTH_BUFFER_BIT);

    // set up draw shader
    if (this.progDraw.invalid) {
      this.progDraw.invalid = false;
      this.progDraw.use();
      this.progDraw.setI1("u_texture", 0);
    }

    // bind the texture
    this.texture.bound = this.texture.bound || this.texture.bind(0);

    // draw scene
    this.bufQuad.draw();
  }
}

interface Rt1InOneWeekend {
  title: string;
  imagePng: string;
}
