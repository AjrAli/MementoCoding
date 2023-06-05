import { TypeDto } from "../typedto";

export class SchoolDto extends TypeDto {
  id: number;
  name: string;
  adress: string;
  town: string;
  description: string;
  constructor(typeInstance?: string) {
    if (typeInstance) {
      super(typeInstance);
    } else {
      super('SchoolDto');
    }
    this.id = 0;
    this.name = '';
    this.adress = '';
    this.town = '';
    this.description = '';
  }
}