export interface StudentPriorities {
    studentName: string;
    coursesPriorities: CoursesPriorities;
}

export interface CoursesPriorities {
    [key: string]: {
        [key: string]: number;
    };
}