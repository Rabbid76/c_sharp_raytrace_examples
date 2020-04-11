import { PixelData } from "../../shared/model/type_models";

export interface RayTraceModel {
  title: string;
  imagePng: string;
  scenes: string[];
  parameter: RayTraceParameter;
}

export interface RayTraceParameter {
  sceneName: string;
  width: number;
  height: number;
  samples: number;
  updateRate: number;
}

export interface RayTraceImageData {
  pixelData: PixelData[];
  progress: number;
}
