import { Component, OnInit } from '@angular/core';
//import { Inject } from '@angular/core';
//import { HttpClient } from '@angular/common/http';
import { RayTraceModel } from '../services/model/raytracemodels';
import { RayTraceService } from '../services/raytrace.service';

@Component({
  selector: 'app-raytrace',
  templateUrl: './raytrace.component.html',
  styleUrls: ['./raytrace.component.css']
})
export class RayTraceComponent implements OnInit {
  public viewvalid: boolean;
  public title = "";
  public raytrace: RayTraceModel;

  constructor(
    private service: RayTraceService,
    //private http: HttpClient,
    //@Inject('BASE_URL') private baseUrl: string
  ) {}

  //ngOnChanges, ngOnInit, ngDoChecks, ngAfterContentInit, ngAfterCcontentChecked, ngAfterViewinit, ngAfterViewChecked, ngOnDestroy

  ngOnInit(): void {
    this.service.initRayTrace((raytrace: RayTraceModel) => {
      this.raytrace = raytrace;
      this.title = this.raytrace.title;
      this.viewvalid = true;
    });
  }
}

