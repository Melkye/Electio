import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AdminService } from '../app/services/admin.service';
import { AuthGuard } from '../app/guards/auth.guard';
import { RoleGuard } from '../app/guards/role.guard';
import { JwtInterceptor } from '../app/interceptors/jwt.interceptor';


import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDialogModule } from '@angular/material/dialog';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CourseListComponent } from './components/course-list/course-list.component';
import { CourseDetailComponent } from './components/course-detail/course-detail.component';
import { StudentListComponent } from './components/student-list/student-list.component';
import { StudentDetailComponent } from './components/student-detail/student-detail.component';
import { StudentCreateComponent } from './components/student-create/student-create.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoadingDialogComponent } from './components/loading-dialog/loading-dialog.component';
import { CourseFormComponent } from './components/course-form/course-form.component';
import { LoginComponent } from './components/login/login.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    CourseListComponent,
    CourseDetailComponent,
    StudentListComponent,
    StudentDetailComponent,
    StudentCreateComponent,
    HeaderComponent,
    FooterComponent, 
    LoadingDialogComponent,
    CourseFormComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    RouterModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatFormFieldModule,
    MatTableModule,
    FormsModule,
    ReactiveFormsModule,
    MatPaginator,
    MatInputModule,
    MatSelectModule,
    MatSortModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatDialogModule,

  ],
  providers: [
    AdminService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    AuthGuard,
    RoleGuard,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
