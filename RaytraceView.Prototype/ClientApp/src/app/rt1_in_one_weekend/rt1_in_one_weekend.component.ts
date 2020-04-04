import { Component, OnInit } from '@angular/core';
//import { Inject } from '@angular/core';
//import { HttpClient } from '@angular/common/http';
import { Rt1InOneWeekend } from '../services/model/rt1_models';
import { Rt1Service } from '../services/rt1_raytrace.service';

@Component({
  selector: 'app-rt1_in_one_weekend',
  templateUrl: './rt1_in_one_weekend.component.html',
})
export class Rt1InOneWeekendComponent implements OnInit {
  public raytrace: Rt1InOneWeekend;

  constructor(
    private service: Rt1Service,
    //private http: HttpClient,
    //@Inject('BASE_URL') private baseUrl: string
  ) {}

  //ngOnChanges, ngOnInit, ngDoChecks, ngAfterContentInit, ngAfterCcontentChecked, ngAfterViewinit, ngAfterViewChecked, ngOnDestroy

  ngOnInit(): void {
    this.service.initRayTrace((raytrace: Rt1InOneWeekend) => {
      this.raytrace = raytrace;
    });
  }
}

