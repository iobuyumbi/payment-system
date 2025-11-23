import { useState } from 'react'
import LocationProfileForm from './blocks/LocationProfileForm'
import { Content } from '../../../_metronic/layout/components/content'
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { useParams } from 'react-router-dom';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';

const breadCrumbs: Array<PageLink> = [
  {
    title: 'Dashboard',
    path: '/dashboard',
    isSeparator: false,
    isActive: true,
  },
  {
    title: '',
    path: '',
    isSeparator: true,
    isActive: true,
  },
  {
    title: 'Locations',
    path: '/locations',
    isSeparator: false,
    isActive: true,
  },
  {
    title: '',
    path: '',
    isSeparator: true,
    isActive: true,
  },
];

const AddLocation = () => {
  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add Location" : "Edit Location");

  return (
    <Content>
      <PageTitleWrapper />
      <PageTitle breadcrumbs={breadCrumbs}>{title}</PageTitle>
      <LocationProfileForm isAdd={ id == null ? true : false}  />
    </Content>
  )
}

export default AddLocation
