import { EntityUrl } from "src/app/enum/entity";
import { BaseDto } from "../utilities/base-dto";

export class SchoolDto extends BaseDto {
  id: number;
  name: string;
  adress: string;
  town: string;
  description: string;
  constructor(typeInstance?: string) {
    if (typeInstance) {
      super(typeInstance, EntityUrl.Schools);
    } else {
      super('SchoolDto', EntityUrl.Schools);
    }
    this.id = 0;
    this.name = '';
    this.adress = '';
    this.town = '';
    this.description = '';
  }
}