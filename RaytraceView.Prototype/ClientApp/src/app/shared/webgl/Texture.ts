export class Texture {
  private size: number[];
  private dummyObj: any;
  public textureObj: any;

  constructor(public readonly gl: any, private image: any, private texUnit: number = 0, trasnformPowerOf2: boolean = false) {
    if (trasnformPowerOf2) {
      let cx = 1 << 31 - Math.clz32(this.image.naturalWidth);
      if (cx < this.image.naturalWidth) cx *= 2;
      let cy = 1 << 31 - Math.clz32(this.image.naturalHeight);
      if (cy < this.image.naturalHeight) cy *= 2;
      var canvas = document.createElement('canvas');
      canvas.width = cx;
      canvas.height = cy;
      var context = canvas.getContext('2d');
      context.drawImage(this.image, 0, 0, canvas.width, canvas.height);
      this.size = [cx, cy];
      this.textureObj = this.createTexture2D(canvas, true);
    } else {
      this.size = [this.image.naturalWidth, this.image.naturalHeight];
      this.textureObj = this.createTexture2D(this.image, true);
    }
  }
  delete(): void {
    this.gl.deleteTexture(this.textureObj);
  }
  createTexture2D(image: any, flipY: boolean): any {
    let t = this.gl.createTexture();
    this.gl.activeTexture(this.gl.TEXTURE0 + this.texUnit);
    this.gl.bindTexture(this.gl.TEXTURE_2D, t);
    this.gl.pixelStorei(this.gl.UNPACK_FLIP_Y_WEBGL, flipY != undefined && flipY == true);
    if (image.cx && image.cy && image.plane)
      this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, image.cx, image.cy, 0, this.gl.RGBA, this.gl.UNSIGNED_BYTE, new Uint8Array(image.plane));
    else
      this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, this.gl.RGBA, this.gl.UNSIGNED_BYTE, image);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MAG_FILTER, this.gl.LINEAR);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MIN_FILTER, this.gl.LINEAR);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_S, this.gl.REPEAT);
    this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_T, this.gl.REPEAT);
    this.gl.bindTexture(this.gl.TEXTURE_2D, null);
    this.gl.activeTexture(this.gl.TEXTURE0);
    return t;
  }
  bind(texUnit: number): boolean {
    let unit = texUnit || this.texUnit;
    this.gl.activeTexture(this.gl.TEXTURE0 + unit);
    if (this.textureObj) {
      this.gl.bindTexture(this.gl.TEXTURE_2D, this.textureObj);
      return true;
    }
    this.gl.bindTexture(this.gl.TEXTURE_2D, this.dummyObj);
    return false;
  }
  bindDflt(texUnit: number): boolean {
    this.gl.activeTexture(this.gl.TEXTURE0 + texUnit);
    this.gl.bindTexture(this.gl.TEXTURE_2D, this.dummyObj);
    return false;
  }
};
