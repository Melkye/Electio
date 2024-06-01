import { Student } from './student.model';

export interface CourseEnrollment {
    id: string;
    title: string;
    quota: number;
    students: Student[];
  }
  