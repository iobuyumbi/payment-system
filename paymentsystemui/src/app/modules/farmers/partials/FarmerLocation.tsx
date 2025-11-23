import GoogleMapReact from 'google-map-react';
import { KTIcon } from '../../../../_metronic/helpers'
import { Content } from '../../../../_metronic/layout/components/content'

const AnyReactComponent = ({ text }: any) => <div>{text}</div>;

const FarmerLocation = () => {
  const defaultProps = {
    center: {
      lat: 1.2921,
      lng: 36.8219
    },
    zoom: 8
  };

  return (
    <Content>
      <div className='card shadow-none rounded-0'>
        <div className='card-header' id='kt_activities_header'>
          <h3 className='card-title fw-bolder text-gray-900'>Location</h3>

          <div className='card-toolbar'>
            <a
              href='#'
              className='btn btn-sm btn-primary me-3'
              data-bs-toggle='modal'
              data-bs-target='#kt_modal_offer_a_deal'
            >
              <KTIcon iconName='pencil' className='fs-6' iconType='outline' /> Edit
            </a>
          </div>
        </div>
        <div className='card-body position-relative' id='kt_activities_body'>
          <div style={{ height: '100vh', width: '100%' }}>
            <GoogleMapReact
              bootstrapURLKeys={{ key: "AIzaSyB50iVvL18un1MJaLkstfcd1kTKTuW-GKQ" }}
              defaultCenter={defaultProps.center}
              defaultZoom={defaultProps.zoom}
            >
              <AnyReactComponent
                lat={59.955413}
                lng={30.337844}
                text="My Marker"
              />
            </GoogleMapReact>
          </div>
        </div>

      </div>
    </Content>
  )
}

export default FarmerLocation
