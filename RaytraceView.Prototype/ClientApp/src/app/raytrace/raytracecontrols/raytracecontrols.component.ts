import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RayTraceService } from '../../services/raytrace.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RayTraceParameter } from '../../services/model/raytracemodels';

@Component({
  selector: 'app-raytrace-controls',
  templateUrl: './raytracecontrols.component.html',
  styleUrls: ['./raytracecontrols.component.css']
})
export class RayTraceControlsComponent {
  form: FormGroup;
  scenes: string[];

  constructor(
    private service: RayTraceService,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    this.scenes = this.service.raytrace.scenes;
    this.form = new FormGroup({
      sceneName: new FormControl(this.service.raytrace.parameter.sceneName, Validators.required),
      width: new FormControl(this.service.raytrace.parameter.width, Validators.required),
      height: new FormControl(this.service.raytrace.parameter.height, Validators.required),
      samples: new FormControl(this.service.raytrace.parameter.samples, Validators.required),
      updaterate: new FormControl(this.service.raytrace.parameter.updateRate * 100, Validators.required),
    }, null, null);
    this.service.controls = this;
  }

  onSubmit() {
    const parameter: RayTraceParameter =
    {
      sceneName: this.form.get("sceneName").value,
      width: +this.form.get("width").value,
      height: +this.form.get("height").value,
      samples: +this.form.get("samples").value,
      updateRate: +this.form.get("updaterate").value / 100
    };
    this.service.startRayTrace(parameter);
  }
}
