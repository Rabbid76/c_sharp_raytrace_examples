import { Component, OnInit } from '@angular/core';
//import { Inject } from '@angular/core';
import { Input, ViewChild, ElementRef } from '@angular/core';
//import { HttpClient } from '@angular/common/http';
import { Rt1InOneWeekend } from '../../services/model/rt1_models'
import { Rt1Service } from '../../services/rt1_raytrace.service';
import { RayTraceView } from "../../shared/interfaces/IRayTraceView";

@Component({
  selector: 'app-rt1_in_one_weekend-rt1_view',
  templateUrl: './rt1_view.component.html',
})
export class Rt1View implements OnInit, RayTraceView {
  @Input() raytrace: Rt1InOneWeekend;
  @ViewChild('ray_trace_canvas', { static: true }) raytracecanvas: ElementRef<HTMLCanvasElement>;
  public progressText: string;

  constructor(
    private service: Rt1Service,
    //private http: HttpClient,
    //@Inject('BASE_URL') private baseUrl: string
  ) { }

  getCanvas = (): HTMLCanvasElement => this.raytracecanvas.nativeElement;

  setProgress(progress: number): void {
    this.progressText = (Math.round(progress * 100000) / 1000).toString() + "%";
  }

  ngOnInit(): void {
    this.service.view = this;
    this.service.initView(this.raytrace.imagePng);
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

