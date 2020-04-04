import { PixelData } from "../../shared/model/type_models";

export interface RayTraceView {
  getCanvas(): HTMLCanvasElement;
  updateView(setPixel: (pixeldata: PixelData) => void): void;
  startRayTrace(): void;
}

