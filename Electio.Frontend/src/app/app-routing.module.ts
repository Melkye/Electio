import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';
import { LoginComponent } from './components/login/login.component';
import { StudentCreateComponent } from './components/student-create/student-create.component';
// import { StudentComponent } from './components/student/student.component';
import { CourseListComponent } from './components/course-list/course-list.component';
import { CourseDetailComponent } from './components/course-detail/course-detail.component';
import { StudentListComponent } from './components/student-list/student-list.component';
import { StudentDetailComponent } from './components/student-detail/student-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'courses', component: CourseListComponent },
  { path: 'courses/:id', component: CourseDetailComponent },
  { path: 'students', component: StudentListComponent },
  { path: 'students/:id', component: StudentDetailComponent },
  { path: 'student-create', component: StudentCreateComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  // { path: 'student (student-detail?)', component: StudentComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Student' } },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }