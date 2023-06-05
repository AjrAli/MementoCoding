import { StudentDto } from './studentdto';

export class GetStudentDto extends StudentDto {
  haschildren: boolean;
  parentname: string;
  constructor() {
    super('GetStudentDto');
    this.haschildren = false;
    this.parentname = '';
  }
}