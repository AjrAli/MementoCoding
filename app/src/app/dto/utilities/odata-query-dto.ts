import { Entity } from "src/app/enum/entity";
import { SchoolProperties } from "src/app/enum/school-properties";
import { StudentProperties } from "src/app/enum/student-properties";

export class ODataQueryDto {
    top: string = '0';
    skip: string = '0';
    orderBy: string[] = [];
    keywords: string[] = [];
    filter: { key: string, value: any }[] = [];
    select: SchoolProperties[] | StudentProperties[] = [];
    expand: Entity[] = [];
    count: boolean = true;
    search: string = '';
    format: string = '';
    inlinecount: string = 'allpages';
    skiptoken: string = '0';

    arrayToCommaString(array: any[]): string {
        if (array.length === 0) {
            return '';
        }

        return array.join(',');
    }

    filterKeywordsInFields(array?: { key: string, value: any }[], words?: string[]): string {
        if (array && array.length > 0) {
            const queryFilter = '$filter= ';
            if (words && words.length > 0) {
                const conditions: string[] = [];

                words.forEach((k) => {
                    const fieldConditions: string[] = [];
                    array.forEach((f) => {
                        if (typeof f.value !== 'number')
                            fieldConditions.push(`contains(${f.key}, '${k}')`);
                        else {
                            if (!isNaN(Number(k))) {
                                fieldConditions.push(`${f.key} eq ${k}`);
                            }
                        }
                        // substringof('5', PhoneNumber)
                    });
                    conditions.push(`(${fieldConditions.join(' or ')})`);
                });

                return queryFilter + conditions.join(' and ');
            }
            const conditions: string[] = [];
            array.forEach((f) => {
                conditions.push(`${f.key} eq ${f.value}`);
            });
            return queryFilter + conditions.join(' or ');
        }

        return '';
    }

    toString(): string {
        const queryString = [
            this.top !== '0' ? `$top=${this.top}` : '',
            this.skip !== '0' ? `$skip=${this.skip}` : '',
            this.orderBy.length > 0 ? `$orderby=${this.arrayToCommaString(this.orderBy)}` : '',
            `${this.filterKeywordsInFields(this.filter, this.keywords)}`,
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
