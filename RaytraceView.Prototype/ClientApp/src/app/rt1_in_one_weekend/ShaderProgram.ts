export interface ShaderSpec {
  source: string;
  stage: any;
}

export class ShaderProgram {
  readonly progObj: any;
  readonly shaderObjs: any[] = [];
  readonly attrInx: any;
  readonly uniLoc: any;

  constructor(public readonly gl: any, shaderList: ShaderSpec[]) {
    for (let i_sh = 0; i_sh < shaderList.length; ++i_sh) {
      let shderObj = this.compile(shaderList[i_sh].source, shaderList[i_sh].stage);
      if (shderObj) this.shaderObjs.push(shderObj);
    }
    this.progObj = this.link(this.shaderObjs)
    if (this.progObj) {
      this.attrInx = {};
      var noOfAttributes = this.gl.getProgramParameter(this.progObj, this.gl.ACTIVE_ATTRIBUTES);
      for (var i_n = 0; i_n < noOfAttributes; ++i_n) {
        var name = this.gl.getActiveAttrib(this.progObj, i_n).name;
        this.attrInx[name] = this.gl.getAttribLocation(this.progObj, name);
      }
      this.uniLoc = {};
      var noOfUniforms = this.gl.getProgramParameter(this.progObj, this.gl.ACTIVE_UNIFORMS);
      for (var i_n = 0; i_n < noOfUniforms; ++i_n) {
        var name = this.gl.getActiveUniform(this.progObj, i_n).name;
        this.uniLoc[name] = this.gl.getUniformLocation(this.progObj, name);
      }
    }
  }

  delete(): void {
    this.shaderObjs.forEach(shaderObj => { this.gl.deleteShader(shaderObj); });
    this.gl.deleteProgram(this.progObj);
  }

  attrI(name: string): number { return this.attrInx[name]; }
  uniformL(name: string): number { return this.uniLoc[name]; }
  use(): void { this.gl.useProgram(this.progObj); }
  setI1(name: string, val: number): void { if (this.uniLoc[name]) this.gl.uniform1i(this.uniLoc[name], val); }
  setF1(name: string, val: number): void { if (this.uniLoc[name]) this.gl.uniform1f(this.uniLoc[name], val); }
  setF2(name: string, arr: number[]): void { if (this.uniLoc[name]) this.gl.uniform2fv(this.uniLoc[name], arr); }
  setF3(name: string, arr: number[]): void { if (this.uniLoc[name]) this.gl.uniform3fv(this.uniLoc[name], arr); }
  setF4(name: string, arr: number[]): void { if (this.uniLoc[name]) this.gl.uniform4fv(this.uniLoc[name], arr); }
  setM33(name: string, mat: number[]): void { if (this.uniLoc[name]) this.gl.uniformMatrix3fv(this.uniLoc[name], false, mat); }
  setM44(name: string, mat: number[]): void { if (this.uniLoc[name]) this.gl.uniformMatrix4fv(this.uniLoc[name], false, mat); }

  compile(source: string, shaderStage: any): any {
    let shaderScript: any = document.getElementById(source);
    if (shaderScript)
      source = shaderScript.text;
    let shaderObj = this.gl.createShader(shaderStage);
    this.gl.shaderSource(shaderObj, source);
    this.gl.compileShader(shaderObj);
    let status = this.gl.getShaderParameter(shaderObj, this.gl.COMPILE_STATUS);
    if (!status) alert(this.gl.getShaderInfoLog(shaderObj));
    return status ? shaderObj : null;
  }

  link(shaderObjs: any[]): any {
    let prog = this.gl.createProgram();
    for (let i_sh = 0; i_sh < shaderObjs.length; ++i_sh)
      this.gl.attachShader(prog, shaderObjs[i_sh]);
    this.gl.linkProgram(prog);
    status = this.gl.getProgramParameter(prog, this.gl.LINK_STATUS);
    if (!status) alert(this.gl.getProgramInfoLog(prog));
    return status ? prog : null;
  }
}
