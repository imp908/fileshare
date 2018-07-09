import { ApplistModule } from './applist.module';

describe('ApplistModule', () => {
  let applistModule: ApplistModule;

  beforeEach(() => {
    applistModule = new ApplistModule();
  });

  it('should create an instance', () => {
    expect(applistModule).toBeTruthy();
  });
});
