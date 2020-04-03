import { Injectable } from '@angular/core';
import { Rt1Controls } from '../rt1_in_one_weekend/rt1_controls/rt1_controls.component';
import { Rt1View } from '../rt1_in_one_weekend/rt1_view/rt1_view.component';

@Injectable()
export class Rt1Service {
  public controls: Rt1Controls = null;
  public view: Rt1View = null;

  constructor() { }

  startRayTrace(): void {
    if (this.view) {
      this.view.startRayTrace();
    }
  }
}
