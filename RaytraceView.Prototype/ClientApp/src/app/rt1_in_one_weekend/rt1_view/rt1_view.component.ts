import { Component, Inject, OnInit } from '@angular/core';
import { Input, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Rt1InOneWeekend, Rt1InOneWeekendImageData } from '../../services/model/rt1_models'
import { Rt1Service } from '../../services/rt1_raytrace.service';
import { RayTraceView } from "../../shared/interfaces/IRayTraceView";
import { PixelData } from "../../shared/model/type_models";

@Component({
  selector: 'app-rt1_in_one_weekend-rt1_view',
  templateUrl: './rt1_view.component.html',
})
export class Rt1View implements OnInit, RayTraceView {
  @Input() raytrace: Rt1InOneWeekend;
  @ViewChild('ray_trace_canvas', { static: true }) raytracecanvas: ElementRef<HTMLCanvasElement>;
  private progressText: string;

  constructor(
    private service: Rt1Service,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  getCanvas = (): HTMLCanvasElement => this.raytracecanvas.nativeElement;

  updateView(setPixel: (pixeldata: PixelData) => void): void {
    this.http.get<Rt1InOneWeekendImageData>(this.baseUrl + 'rt1inoneweekendimagedata').subscribe(result => {
      const pixelData = result.pixelData;
      if (pixelData) {
        for (let i = 0; i < pixelData.length; ++i) {
          setPixel(pixelData[i]);
        }
        this.progressText = (Math.round(result.progress * 100000) / 1000).toString() + "%";
      }
    }, error => console.error(error));
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

  public startRayTrace(): void {
    this.http.get<Rt1InOneWeekend>(this.baseUrl + 'rt1inoneweekend').subscribe(result => {
      const raytrace = result;
      this.service.initView(raytrace.imagePng);
    }, error => console.error(error));
  }

  resize(): void {
    // [...]
  }
}

