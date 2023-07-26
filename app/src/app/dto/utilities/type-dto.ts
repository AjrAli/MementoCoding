import { Entity } from "src/app/enum/entity";


export class TypeDto {
    typeInstance: string;
    urlEntity: Entity;
    [key: string]: any;
    constructor(typeInstance: string, urlEntity:Entity){
        this.typeInstance = typeInstance;
        this.urlEntity = urlEntity;
    }
  }