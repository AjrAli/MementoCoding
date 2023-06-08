import { StudentDto } from './student-dto';

export class GetStudentDto extends StudentDto {
  haschildren: boolean;
  parentname: string;
  constructor() {
    super('GetStudentDto');
    this.haschildren = false;
    this.parentname = '';
  }
}