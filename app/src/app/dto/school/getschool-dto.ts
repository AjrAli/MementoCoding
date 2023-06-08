import { SchoolDto } from './school-dto';

export class GetSchoolDto extends SchoolDto {
  haschildren: boolean;
  parentname: string;
  constructor() {
    super('GetSchoolDto');
    this.haschildren = false;
    this.parentname = '';
  }
}