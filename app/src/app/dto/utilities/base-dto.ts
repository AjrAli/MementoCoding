import { Entity } from "src/app/enum/entity";
import { TypeDto } from "./type-dto";

export class BaseDto extends TypeDto {
    haschildren: boolean;
    parentname: string;
    constructor(typeInstance: string, urlEntity: Entity) {
      super(typeInstance, urlEntity);
      this.haschildren = false;
      this.parentname = '';
    }
  }