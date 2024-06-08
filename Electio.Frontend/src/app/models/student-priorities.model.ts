export interface StudentPriorities {
    studentName: string;
    coursesPriorities: CoursesPriorities;
}

export interface CoursesPriorities {
    [studyComponent: string]: {
        [course: string]: number;
    };
}