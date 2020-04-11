import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms'

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { RayTraceComponent } from './raytrace/raytrace.component';
import { RayTraceControlsComponent } from './raytrace/raytracecontrols/raytracecontrols.component';
import { RayTraceViewComponent } from './raytrace/raytraceview/raytraceview.component';
import { RayTraceService } from './services/raytrace.service';
import { HealthCheckComponent } from './health-check/health-check.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RayTraceComponent,
    RayTraceControlsComponent,
    RayTraceViewComponent, 
    AboutComponent,
    HealthCheckComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'raytrace', component: RayTraceComponent },
      { path: 'about', component: AboutComponent },
      { path: 'health-check', component: HealthCheckComponent }
    ]),
    ReactiveFormsModule,
  ],
  providers: [RayTraceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
