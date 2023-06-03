import {SchoolDto} from './schooldto';

export class GetSchoolDto extends SchoolDto {
    haschildren: boolean;
    parentname: string;
    constructor(){
      super();
      this.haschildren = false,
      this.parentname = ''
    }
  }