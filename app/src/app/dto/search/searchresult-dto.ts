import { TypeDto } from "../utilities/type-dto";

export enum Entity {
    Students = 'students',
    Schools = 'schools'
}
export class SearchResultDto extends TypeDto {
  id: number;
  title: string;
  subtitle: string;
  description: string;
  type: Entity | null;
  constructor(type?: Entity) {
    super('SearchResultDto');
    this.id = 0;
    this.title = '';
    this.subtitle = '';
    this.description = '';
    this.type = type || null;
  }
}