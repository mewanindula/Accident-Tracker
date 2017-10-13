#include <AFMotor.h>

AF_DCMotor motor1(3, MOTOR12_64KHZ);  

AF_DCMotor motor2(4, MOTOR12_64KHZ);

AF_DCMotor motor3(1, MOTOR12_64KHZ);

AF_DCMotor motor4(2, MOTOR12_64KHZ);

char junk;
String inputString="";

void setup()
{
  Serial.begin(9600);           // set up Serial library at 9600 bps
  Serial.println("Motor test!");
  
  motor1.setSpeed(250);   
  motor2.setSpeed(250);
  motor3.setSpeed(250);
  motor4.setSpeed(250);

}

void loop()
{
  if(Serial.available())
  {
    while(Serial.available())
    {
      char inChar = (char)Serial.read();
      inputString += inChar;
    }
  
    Serial.println(inputString);
  
    while(Serial.available() > 0)
    {
      junk = Serial.read();
    }
  
    if(inputString == "F")
    {
      motor1.run(FORWARD);
      motor2.run(FORWARD);
      motor3.run(FORWARD);
      motor4.run(FORWARD);
      
      //forward();
    }
    else if(inputString == "B")
    {
      motor1.run(BACKWARD);     // the other way
      motor2.run(BACKWARD);  //again for motor 2
      motor3.run(BACKWARD);
      motor4.run(BACKWARD);

      //backward();
    }
  
    else if(inputString == "S")
    {
      motor1.run(RELEASE);      // stopped
      motor2.run(RELEASE);  // command motor 2 to stop
      motor3.run(RELEASE);
      motor4.run(RELEASE);

      //stopVehicle();
    }
  
    else if(inputString == "G") //Forwad Left
    {
      motor1.run(FORWARD);      // stopped
      motor2.run(FORWARD);  // command motor 2 to stop
      motor3.run(RELEASE);
      motor4.run(RELEASE);

      //forwardLeft();
    }
  
    else if(inputString == "H") //Backward Left
    {
      motor1.run(BACKWARD);      // stopped
      motor2.run(BACKWARD);  // command motor 2 to stop
      motor3.run(RELEASE);
      motor4.run(RELEASE);

      //backwardLeft();
    }
  
    else if(inputString == "I") //Forwad Right
    {
      motor1.run(RELEASE);      // stopped
      motor2.run(RELEASE);  // command motor 2 to stop
      motor3.run(FORWARD);
      motor4.run(FORWARD);

      //forwardRight();
    }
  
    else if(inputString == "J") //Backward Right
    {
      motor1.run(RELEASE);      // stopped
      motor2.run(RELEASE);  // command motor 2 to stop
      motor3.run(BACKWARD);
      motor4.run(BACKWARD);

      //backwardRight();
    }

    
  
    inputString = "";
  }
  
}

/*void forward()
{
  motor1.run(FORWARD);
  motor2.run(FORWARD);
  motor3.run(FORWARD);
  motor4.run(FORWARD);
}

void backward()
{
  motor1.run(BACKWARD);     // the other way
  motor2.run(BACKWARD);  //again for motor 2
  motor3.run(BACKWARD);
  motor4.run(BACKWARD);
}

void stopVehicle()
{
  motor1.run(RELEASE);      // stopped
  motor2.run(RELEASE);  // command motor 2 to stop
  motor3.run(RELEASE);
  motor4.run(RELEASE);
}

void forwardLeft()
{
  motor1.run(FORWARD);      // stopped
  motor2.run(FORWARD);  // command motor 2 to stop
  motor3.run(RELEASE);
  motor4.run(RELEASE);
}

void backwardLeft()
{
  motor1.run(BACKWARD);      // stopped
  motor2.run(BACKWARD);  // command motor 2 to stop
  motor3.run(RELEASE);
  motor4.run(RELEASE);
}
  
void forwardRight()
{
  motor1.run(RELEASE);      // stopped
  motor2.run(RELEASE);  // command motor 2 to stop
  motor3.run(FORWARD);
  motor4.run(FORWARD);
}
  
void backwardRight()
{
  motor1.run(RELEASE);      // stopped
  motor2.run(RELEASE);  // command motor 2 to stop
  motor3.run(BACKWARD);
  motor4.run(BACKWARD);
}

/*long vibMesh()
{
  delay(10);

  long vib = pulseIn(EP, HIGH);

  return vib;
}*/

