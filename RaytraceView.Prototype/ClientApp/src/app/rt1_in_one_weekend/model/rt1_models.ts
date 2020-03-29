export interface Rt1InOneWeekend {
  title: string;
  imagePng: string;
}

export interface Rt1InOneWeekendImageData {
  pixelData: PixelData[];
  progress: number;
}

export interface PixelData {
  x: number;
  y: number;
  r: number;
  g: number;
  b: number;
}
