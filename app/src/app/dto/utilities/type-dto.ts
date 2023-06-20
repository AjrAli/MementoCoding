export class TypeDto {
    typeInstance: string;
    [key: string]: any;
    constructor(typeInstance: string){
        this.typeInstance = typeInstance;
    }
  }