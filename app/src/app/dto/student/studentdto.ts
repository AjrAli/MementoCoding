export class StudentDto {
    id: number;
    firstName: string;
    lastName: string;
    age: number;
    adress: string;
    schoolId: number;
    constructor(){
      this.id = 0,
      this.firstName = '',
      this.lastName = '',
      this.age = 0,
      this.adress = '',
      this.schoolId = 0
    }
  }