export interface VertexAttribute {
  data: number[];
  attrSize: number;
  attrLoc: number;
}

interface VertexAttributeSpecification {
  size: number;
  loc: number;
  no_of: number
}

export class VertexBuffer {
  readonly attr: VertexAttributeSpecification[] = [];
  readonly inxLen: number;
  readonly primitive_type: any;
  readonly vbos: object[] = [];
  readonly ibo: object;

  constructor(private readonly gl: any, attribs: VertexAttribute[], indices: number[], type: any = null) {
    this.inxLen = indices ? indices.length : 0;
    this.primitive_type = type ? type : this.gl.TRIANGLES;
    for (let i = 0; i < attribs.length; ++i) {
      this.vbos.push(gl.createBuffer());
      this.attr.push({ size: attribs[i].attrSize, loc: attribs[i].attrLoc, no_of: attribs[i].data.length / attribs[i].attrSize });
      this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.vbos[i]);
      this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(attribs[i].data), this.gl.STATIC_DRAW);
    }
    this.gl.bindBuffer(gl.ARRAY_BUFFER, null);
    if (this.inxLen > 0) {
      this.ibo = gl.createBuffer();
      this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.ibo);
      this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(indices), this.gl.STATIC_DRAW);
      this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, null);
    }
  }

  delete(): void {
    if (this.ibo) this.gl.deleteBuffer(this.ibo);
    this.vbos.forEach(vbo => { this.gl.deleteBuffer(vbo); });
  }

  draw(): void {
    for (let i = 0; i < this.vbos.length; ++i) {
      this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.vbos[i]);
      this.gl.vertexAttribPointer(this.attr[i].loc, this.attr[i].size, this.gl.FLOAT, false, 0, 0);
      this.gl.enableVertexAttribArray(this.attr[i].loc);
    }
    if (this.inxLen > 0) {
      this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.ibo);
      this.gl.drawElements(this.primitive_type, this.inxLen, this.gl.UNSIGNED_SHORT, 0);
      this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, null);
    }
    else
      this.gl.drawArrays(this.primitive_type, 0, this.attr[0].no_of);
    for (let i = 0; i < this.vbos.length; ++i)
      this.gl.disableVertexAttribArray(this.attr[i].loc);
    this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
  }
}
