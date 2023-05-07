import React, { useState, useEffect } from "react";
import { Button, Modal, Table, Form } from "react-bootstrap";
import Exit from "../Exit.tsx";
import { Member } from "../../models/User.tsx";
import { useLocation } from 'react-router-dom';
import { handleAddStoreManagerPermission,handleRemoveStoreManagerPermission, handleEmployeesOfStore
,handleAppointStoreOwner,handleAppointStoreManager } from "../../actions/MemberActions.tsx";
import { ResponseT,Response } from "../../models/Response.tsx";

const ManageStoreEmployeesPage = (props) => {

  const location = useLocation();
  const { userId, storeId } = location.state;
  const [employees, setEmployees] = useState<Member[]>([]);

  const [showEditModal, setShowEditModal] = useState(false);
  const [editPermission, setEditPermission] = useState<string>('');

  const [showAddModal, setShowAddModal] = useState(false);
  const [appointEmail, setAppointEmail] = useState<string>('');
  const [appointType, setAppointType] = useState<string>('Appintment owner');
  
  const [selectedEmployee, setSelectedEmployee] = useState<Member>();
  const [getEmployeesResponse, setGetEmployeesResponse]=useState<ResponseT>();

  const [editPermissionResponse, setEditPermissionResponse]=useState<Response>();
  const [addEmployeeResponse, setAddEmployeeResponse]=useState<Response>();

  const getStoreEmployees =()=>{
    handleEmployeesOfStore(userId,storeId).then(
      value => {
        setGetEmployeesResponse(value as ResponseT);
      })
      .catch(error => alert(error));
  }

  useEffect(() => {
    getStoreEmployees();
 }, [])

useEffect(() => {
  if(getEmployeesResponse !=undefined){
    getEmployeesResponse?.errorOccured ? alert(getEmployeesResponse?.errorMessage) : setEmployees(getEmployeesResponse?.value as Member[]);
  }
}, [getEmployeesResponse])

  const handleEditModalClose = () => {setShowEditModal(false); setEditPermission('');}

  const handleAddModalClose = () => {setShowAddModal(false); setAppointEmail(''); setAppointType('Appintment owner');}
  const handleAddModalShow = () => setShowAddModal(true);


  const handleAddPermission = (event) => {
    event.preventDefault();
    handleAddStoreManagerPermission(userId, storeId,selectedEmployee?.email, editPermission ).then(
      value => {
        
        setEditPermissionResponse(value as Response);
      })
      .catch(error => alert(error));
    
  };

  const handleRemovePermission = (event) => {
    event.preventDefault();
    handleRemoveStoreManagerPermission(userId, storeId,selectedEmployee?.email, editPermission ).then(
      value => {
        
        setEditPermissionResponse(value as Response);
      })
      .catch(error => alert(error));
  };

  const handleEditPermission = (employee) => {
    setSelectedEmployee(employee);
    setShowEditModal(true);
  };

  useEffect(() => {
    if(editPermissionResponse !=undefined){
      editPermissionResponse?.errorOccured ? alert(editPermissionResponse?.errorMessage) : EditPermissionSuccess();
    }
  }, [editPermissionResponse])
  
  const EditPermissionSuccess=()=>{
    getStoreEmployees();
    handleEditModalClose();
  }

  const handleAddEmployee = (event) => {
    event.preventDefault();
    if(appointType==="Appintment owner"){
      handleAppointStoreOwner(userId, storeId,appointEmail).then(
        value => {
          
          setAddEmployeeResponse(value as Response);
        })
        .catch(error => alert(error));
    }
    else if(appointType==="Appintment store manager"){
      handleAppointStoreManager(userId, storeId,appointEmail).then(
        value => {
          setAddEmployeeResponse(value as Response);
        })
        .catch(error => alert(error));
    }
  };

  useEffect(() => {
    if(addEmployeeResponse !=undefined){
      addEmployeeResponse?.errorOccured ? alert(addEmployeeResponse?.errorMessage) : AddEmployeeSuccess();
    }
  }, [addEmployeeResponse])
  
  const AddEmployeeSuccess=()=>{
    handleAddModalClose();
    getStoreEmployees(); 
  }

  const handlePermissionChange = (event) => {
    setEditPermission(event.target.value);
  }

  const handleAppointTypeChange = (event) => {
    setAppointType(event.target.value);
  }

  const handleAppointEmailChange = (event) => {
    setAppointEmail(event.target.value);
  }
  

  return (
    <div>
      <Exit id={props.id}/>

      <h1>Employees</h1>
      <Button variant="primary" onClick={handleAddModalShow} style={{margin: "0.5rem"}}>
        Add New Employee
      </Button>

      <Modal show={showAddModal} onHide={() =>handleAddModalClose()}>
      <Modal.Header closeButton>
        <Modal.Title>Add Employee</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form >
          <Form.Group controlId="email">
            <Form.Label>Employee email in system</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter employee email"
              value={appointEmail}
              onChange={handleAppointEmailChange}
            />
          </Form.Group>
          <span className="fs-2">Choose appintment type:</span>
          <Form.Control as="select" value={appointType} onChange={handleAppointTypeChange}>
            <option value="Appintment owner">Appintment owner</option>
            <option value="Appintment store manager">Appintment store manager</option>
          </Form.Control>
          <Button variant="primary" style={{margin: "0.5rem"}} onClick={handleAddEmployee}>
            Add
          </Button>
        </Form>
      </Modal.Body>
      </Modal>

      <Table striped bordered hover>
        <thead>
          <tr>
          <th>First Name</th>
          <th>Last Name</th>
          <th>Email</th>
          <th>Permissions</th>
          </tr>
        </thead>
        <tbody>
        {employees.map((member) => (
          <tr key={member.id}>
            <td>{member.firstName}</td>
            <td>{member.lastName}</td>
            <td>{member.email}</td>
            <td>{member.permissions?.map((p) => (
              <div> {p} </div>
            ))}</td>
          <td>
            <Button
                  variant="primary"
                  onClick={() => handleEditPermission(member)}
                >
                  Edit Permissions
                </Button>
             </td>
          </tr>
        ))}
      </tbody>
      </Table>
      <Modal show={showEditModal} onHide={() => handleEditModalClose()}>
        <Modal.Header closeButton>
          <Modal.Title>Edit Permissions</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>
            Editing permissions for {selectedEmployee && selectedEmployee.email}
          </p>
         
          <span className="fs-2">Choose Permission:</span>
          <Form.Control as="select" value={editPermission} onChange={handlePermissionChange}>
            <option value="owner permissions">owner permissions</option>
            <option value="founder permissions">founder permissions</option>
            <option value="edit manager permissions">edit manager permissions</option>
            <option value="get store history">get store history</option>
            <option value="add new owner">add new owner</option>
            <option value="remove owner">remove owner</option>
            <option value="add new manager">add new manager</option>
            <option value="get employees info">get employees info</option>
            <option value="product management permissions">product management permissions</option>
          </Form.Control>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => handleEditModalClose()}>
            Cancel
          </Button>
          <Button variant="primary" onClick={handleAddPermission}>
            Add Permission
          </Button>
          <Button variant="primary" onClick={handleRemovePermission}>
            Remove Permission
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default ManageStoreEmployeesPage;
