#include <nRF24L01.h>
#include <printf.h>
#include <RF24.h>
#include <RF24_config.h>
//
// Hardware conf
//

// Set up nRF24L01 radio on SPI bus plus pins 7 & 8 
RF24 radio(7,8);

//
// Topology
//

// Radio pipe addresses for the 2 nodes to communicate.
byte addresses[][6] = {"1Node","2Node"};
// payload configuration
const int max_payload_size = 32;
char receive_payload[max_payload_size+1]; // +1 to allow room for a terminating NULL char
const int LED_PIN = 4;
const int RELAY_PIN = 5;

void setup(void) {
    Serial.begin(57600);
    
    printf_begin();
    printf("\nRemote Switch Arduino\n\r");

    //
    // Setup and configure rf radio
    //
    radio.begin();
    // enable dynamic payloads
    radio.setCRCLength( RF24_CRC_16 ) ;
    radio.enableDynamicPayloads();

    // optionally, increase the delay between retries & # of retries
    radio.setAutoAck( true ) ;
    radio.setPALevel(RF24_PA_MAX);
    radio.setDataRate(RF24_2MBPS);

    radio.setRetries(15,60);
    radio.openWritingPipe(addresses[0]);
    radio.openReadingPipe(1,addresses[1]);
    radio.startListening();
    radio.printDetails();

    // LED setup
    pinMode(LED_PIN, OUTPUT);
    pinMode(RELAY_PIN, OUTPUT); 
    digitalWrite(RELAY_PIN, HIGH);
}

char * read() {
    // Dump the payloads until we've gotten everything
    uint8_t len;
    bool done = false;

    while (radio.available())
    {
        // Fetch the payload, and see if this was the last one.
        len = radio.getDynamicPayloadSize();
        radio.read( receive_payload, len );

        // Put a zero at the end for easy printing
        receive_payload[len] = 0;

        // Spew it
        printf("Got payload size=%i value=%s\n\r",len,receive_payload);
    }

    return receive_payload;
}

void send(char* toSend) {
    // First, stop listening so we can talk
    radio.stopListening();

    // Send the final one back.
    radio.write( toSend, sizeof(toSend) );
    printf("Sent response %s.\n\r", toSend);

    // Now, resume listening so we catch the next packets.
    radio.startListening();
}

void loop(void)
{
  /*digitalWrite(RELAY_PIN,HIGH);
digitalWrite(LED_PIN, LOW);
  delay(200);
  digitalWrite(RELAY_PIN,LOW);
digitalWrite(LED_PIN, HIGH);
  delay(200);*/
            
  
    if ( radio.available() )
    {
        char* receive = read();

        if(strcmp("1", receive) == 0) {
            digitalWrite(LED_PIN, HIGH);
            digitalWrite(RELAY_PIN, LOW);
        }
        else {
            digitalWrite(LED_PIN, LOW);
            digitalWrite(RELAY_PIN, HIGH);
        }

        send(receive);
    }
}
