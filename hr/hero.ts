export class Hero {
  id: number;
  name: string;
  value_:string;

  constructor(
    public id_?:number,public name_?:string ,public value_0?:string
  )
  {
    this.id=id_;this.name=name_;this.value_=value_0;
  }
}

export const HEROES: Hero[] = [
  { id: 11, name: 'Mr. Nice', value_:'aaa' },
  { id: 12, name: 'Narco', value_:'bbb' },
  { id: 13, name: 'Bombasto',value_:undefined },
  { id: 14, name: 'Celeritas', value_:'a' },
  { id: 15, name: 'Magneta', value_:'a' },
  { id: 16, name: 'RubberMan', value_:'a' },
  { id: 17, name: 'Dynama', value_:'a' },
  { id: 18, name: 'Dr IQ', value_:'a' },
  { id: 19, name: 'Magma', value_:'a' },
  { id: 20, name: 'Tornado', value_:'a' }
];
