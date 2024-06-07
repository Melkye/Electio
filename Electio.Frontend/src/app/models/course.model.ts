export interface Course {
    id: string;
    title: string;
    quota: number;
    faculty: number; //string;
    specialties: number[]; //string[];
    studyComponent: string;
  }
  
 export interface AvailableCoursesResponse {
    [studyComponent: string]: Course[];
  }