import { useState, useEffect } from 'react';
import ProjectService from '../../services/ProjectService';

const projectService = new ProjectService();

type ProjectArr = {
  id: number;
  projectName: string;
};

export default function ProjectDropdown(props: any) {
  const { formik, isRequired, isDisabled,isCountryBased } = props
  const [projects, setProjects] = useState<ProjectArr[]>([]);

  const bindProjects = async () => {
    const data = {
      pageNumber: 1,
      pageSize: 10,
      countryId: null
    };
    var response: any = isCountryBased === true ? await projectService.getProjectByCountryData(data) : await projectService.getProjectData(data);

    if (Array.isArray(response)) {
      response.unshift({ id: 0, projectName: 'Select project' });
    } else {
      response = [{ id: 0, projectName: 'Select project' }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setProjects(response);
  };

  useEffect(() => {
    bindProjects();
  }, []);
  useEffect(() => {
    bindProjects();
  }, [formik.values.countryId]);
  return (
    < div className="">
      <label className='fs-6 fw-bold mb-2'>Projects {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}</label>
      <select
        data-placeholder='Select Project'
        name='projectId'
        className='form-select mb-3 mb-lg-0'
        disabled={isDisabled}
        {...formik.getFieldProps('projectId')}
      >

        {projects.map((item) => (
          <option key={item.id} value={item.id}>
            {item.projectName}
          </option>
        ))}
      </select>
      {formik.touched['projectId'] && formik.errors['projectId'] && (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>
            <span role='alert'>{formik.errors['projectId']}</span>
          </div>
        </div>
      )}
    </div>
  )
}
