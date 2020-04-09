import { PixelData } from "../../shared/model/type_models";

export interface Rt1InOneWeekend {
  title: string;
  imagePng: string;
  parameter: Rt1InOneWeekendParameter;
}

export interface Rt1InOneWeekendParameter {
  width: number;
  height: number;
  samples: number;
  updateRate: number;
}

export interface Rt1InOneWeekendImageData {
  pixelData: PixelData[];
  progress: number;
}
