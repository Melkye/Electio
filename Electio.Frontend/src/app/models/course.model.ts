export interface Course {
    id: string;
    title: string;
    quota: number;
    faculty: number; //string;
    specialties: number[]; //string[];
    studyComponent: number //string;
  }
  
 export interface AvailableCoursesResponse {
    [studyComponent: string]: Course[];
  }