export class SchoolDto {
    id: number;
    name: string;
    adress: string;
    town: string;
    description: string;
    constructor(){
      this.id = 0,
      this.name = '',
      this.adress = '',
      this.town = '',
      this.description = ''
    }
  }