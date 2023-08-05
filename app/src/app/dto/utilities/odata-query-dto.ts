import { Entity } from "src/app/enum/entity";
import { SchoolProperties } from "src/app/enum/school-properties";
import { StudentProperties } from "src/app/enum/student-properties";

export class ODataQueryDto {
    top: string = '0';
    skip: string = '0';
    orderBy: string[] = [];
    filter: string[] = [];
    select: SchoolProperties[] | StudentProperties[] = [];
    expand: Entity[] = [];
    count: boolean = true;
    search: string = '';
    format: string = '';
    inlinecount: string = 'allpages';
    skiptoken: string = '0';

    public arrayToCommaString(array: any[]): string {
        if (array.length === 0) {
            return '';
        }

        return array.join(',');
    }

    toString(): string {
        const queryString = [
            this.top !== '0' ? `$top=${this.top}` : '',
            this.skip !== '0' ? `$skip=${this.skip}` : '',
            this.orderBy.length > 0 ? `$orderby=${this.arrayToCommaString(this.orderBy)}` : '',
            this.filter.length > 0 ? `$filter=${this.arrayToCommaString(this.filter)}` : '',
            this.select.length > 0 ? `$select=${this.arrayToCommaString(this.select)}` : '',
            this.expand.length > 0 ? `$expand=${this.arrayToCommaString(this.expand)}` : '',
            this.count !== true ? `$count=${this.count}` : '',
            this.search !== '' ? `$search=${this.search}` : '',
            this.format !== '' ? `$format=${this.format}` : '',
            this.inlinecount !== 'allpages' ? `$inlinecount=${this.inlinecount}` : '',
            this.skiptoken !== '0' ? `$skiptoken=${this.skiptoken}` : ''
        ].filter(Boolean).join('&');

        return queryString;
    }
}
