import {StudentDto} from './studentdto';

export interface GetStudentDto extends StudentDto {
    haschildren: boolean;
    parentname: string;
  }