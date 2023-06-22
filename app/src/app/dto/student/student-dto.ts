import { TypeDto } from "../utilities/type-dto";

export class StudentDto extends TypeDto {
  id: number;
  firstName: string;
  lastName: string;
  age: number;
  adress: string;
  schoolId: number;
  constructor(typeInstance?: string) {
    if (typeInstance) {
      super(typeInstance);
    } else {
      super('StudentDto');
    }
    this.id = 0;
    this.firstName = '';
    this.lastName = '';
    this.age = 0;
    this.adress = '';
    this.schoolId = 0;
  }
}