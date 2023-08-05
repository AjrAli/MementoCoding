import { EntityUrl } from "src/app/enum/entity";
import { TypeDto } from "../utilities/type-dto";

export class SearchResultDto extends TypeDto {
  id: number;
  title: string;
  subtitle: string;
  description: string;
  type: EntityUrl ;
  constructor(type: EntityUrl) {
    super('SearchResultDto', type);
    this.id = 0;
    this.title = '';
    this.subtitle = '';
    this.description = '';
    this.type = type;
  }
}