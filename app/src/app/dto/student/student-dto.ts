import { Entity } from "src/app/enum/entity";
import { BaseDto } from "../utilities/base-dto";

export class StudentDto extends BaseDto {
  id: number;
  firstName: string;
  lastName: string;
  age: number;
  adress: string;
  schoolId: number;
  constructor(typeInstance?: string) {
    if (typeInstance) {
      super(typeInstance, Entity.Students);
    } else {
      super('StudentDto', Entity.Students);
    }
    this.id = 0;
    this.firstName = '';
    this.lastName = '';
    this.age = 0;
    this.adress = '';
    this.schoolId = 0;
  }
}