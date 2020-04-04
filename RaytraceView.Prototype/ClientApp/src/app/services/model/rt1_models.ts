import { PixelData } from "../../shared/model/type_models";

export interface Rt1InOneWeekend {
  title: string;
  imagePng: string;
}

export interface Rt1InOneWeekendImageData {
  pixelData: PixelData[];
  progress: number;
}
