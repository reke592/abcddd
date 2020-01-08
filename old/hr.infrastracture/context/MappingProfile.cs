using AutoMapper;

namespace hr.infrastracture.context {
    public static class MappingProfile {
        public static MapperConfiguration InitializeAutoMapper() {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.AddProfile(new EmployeeMappingProfile());
            });
            
            return config;
        } 
    }
}