import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RayTraceService } from '../../services/raytrace.service';
import { FormBuilder, FormGroup, FormControl, Validators, AbstractControl, ValidatorFn, AsyncValidatorFn } from '@angular/forms';
import { RayTraceParameter } from '../../services/model/raytracemodels';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

function psitiveNonZeroNumber(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const forbidden = Number(control.value) <= 0;
    return forbidden ? { 'lessOrEqualZero': { value: control.value } } : null;
  };
}


@Component({
  selector: 'app-raytrace-controls',
  templateUrl: './raytracecontrols.component.html',
  styleUrls: ['./raytracecontrols.component.css']
})
export class RayTraceControlsComponent {
  form: FormGroup;
  scenes: string[];

  constructor(
    private fb: FormBuilder,
    private service: RayTraceService,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    this.scenes = this.service.raytrace.scenes;
    
    this.form = this.fb.group({
      sceneName: [this.service.raytrace.parameter.sceneName, Validators.required],
      width: [this.service.raytrace.parameter.width, [Validators.required, psitiveNonZeroNumber()]],
      height: [this.service.raytrace.parameter.height, [Validators.required, psitiveNonZeroNumber()]],
      samples: [this.service.raytrace.parameter.samples, [Validators.required, psitiveNonZeroNumber()]],
      updaterate: [this.service.raytrace.parameter.updateRate * 100, Validators.required],
    });
    this.form.setAsyncValidators(this.isInvalidRayTraceModel());

    this.service.controls = this;
  }

  isInvalidRayTraceModel(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
      const parameter: RayTraceParameter =
      {
        sceneName: this.form ? this.form.get("sceneName").value : "",
        width: this.form ? +this.form.get("width").value : 1,
        height: this.form ? +this.form.get("height").value : 1,
        samples: this.form ? +this.form.get("samples").value : 1,
        updateRate: this.form ? +this.form.get("updaterate").value / 100 : 0.1
      };

      var url = this.baseUrl + "raytraceimagedata/IsInvalidRayTraceModel";
      return this.http.post<boolean>(url, parameter).pipe(map(result => {

        return (result ? { isInvalidRayTraceModel: true } : null);
      }));
    }
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
