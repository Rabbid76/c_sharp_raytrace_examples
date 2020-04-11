import { Component, OnInit } from '@angular/core';
//import { Inject } from '@angular/core';
import { Input, ViewChild, ElementRef } from '@angular/core';
//import { HttpClient } from '@angular/common/http';
import { RayTraceModel } from '../../services/model/raytracemodels'
import { RayTraceService } from '../../services/raytrace.service';
import { RayTraceView } from "../../shared/interfaces/IRayTraceView";

@Component({
  selector: 'app-raytrace-view',
  templateUrl: './raytraceview.component.html',
})
export class RayTraceViewComponent implements OnInit, RayTraceView {
  @Input() raytrace: RayTraceModel;
  @ViewChild('ray_trace_canvas', { static: true }) raytracecanvas: ElementRef<HTMLCanvasElement>;
  public progressText: string;

  constructor(
    private service: RayTraceService,
    //private http: HttpClient,
    //@Inject('BASE_URL') private baseUrl: string
  ) { }

  getCanvas = (): HTMLCanvasElement => this.raytracecanvas.nativeElement;

  setProgress(progress: number): void {
    this.progressText = (Math.round(progress * 100000) / 1000).toString() + "%";
  }

  ngOnInit(): void {
    this.service.view = this;
    this.service.initView(this.service.raytrace.imagePng);
  }

  ngOnDestroy(): void {
    this.service.stopRayTrace()
  }

  ngAfterViewInit(): void {
    this.service.initCanvas();
    window.onresize = () => { this.resize(); }
  }

  resize(): void {
    // [...]
  }
}

