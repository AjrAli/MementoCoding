import {SchoolDto} from './schooldto';

export interface GetSchoolDto extends SchoolDto {
    haschildren: boolean;
    parentname: string;
  }