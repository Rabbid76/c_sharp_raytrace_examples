import { Component, Inject } from '@angular/core';
import { Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Rt1Service } from '../../services/rt1_raytrace.service';

@Component({
  selector: 'app-rt1_in_one_weekend-rt1_controls',
  templateUrl: './rt1_controls.component.html',
})
export class Rt1Controls {
  public raytraceview: any;

  constructor(
    private service: Rt1Service,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    this.service.controls = this;
  }

  onApply(): void {
    this.service.startRayTrace();
  }
}
